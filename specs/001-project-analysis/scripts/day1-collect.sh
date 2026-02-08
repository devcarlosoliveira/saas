#!/usr/bin/env bash
set -euo pipefail

ARTIFACT_DIR="specs/001-project-analysis/artifacts"
mkdir -p "$ARTIFACT_DIR"

echo "Starting Day 1 automated collection..."

# Detect repo owner/name
REMOTE_URL=$(git config --get remote.origin.url || true)
GH_REPO=$(echo "$REMOTE_URL" | sed -E 's#.*github.com[:/]+([^/]+/[^/]+)(\.git)?#\1#')
if [ -z "$GH_REPO" ]; then
  echo "Warning: Could not detect GitHub repo automatically (no remote.origin.url)"
fi

echo "Artifact dir: $ARTIFACT_DIR"

echo "Collecting git remote info..."
git remote -v > "$ARTIFACT_DIR/git-remote.txt" || true

echo "Collecting last 30 GitHub Actions runs (if GH repo detected and gh is available)..."
if command -v gh >/dev/null 2>&1 && [ -n "$GH_REPO" ]; then
  gh api "repos/${GH_REPO}/actions/runs?per_page=30" > "$ARTIFACT_DIR/ci-runs.json" || true
elif [ -n "$GITHUB_TOKEN" ] && [ -n "$GH_REPO" ]; then
  curl -sS -H "Authorization: token $GITHUB_TOKEN" "https://api.github.com/repos/${GH_REPO}/actions/runs?per_page=30" \
    > "$ARTIFACT_DIR/ci-runs.json" || true
else
  echo "Skipping CI runs collection: gh not available or GITHUB_TOKEN not set or repo unknown" > "$ARTIFACT_DIR/ci-collection-note.txt"
fi

echo "Running dependency vulnerability check (dotnet)..."
if command -v dotnet >/dev/null 2>&1; then
  dotnet list package --vulnerable > "$ARTIFACT_DIR/dotnet-vulnerable.txt" || true
  dotnet list package > "$ARTIFACT_DIR/dotnet-packages.txt" || true
else
  echo "dotnet not available, skipping dotnet package checks" > "$ARTIFACT_DIR/dotnet-note.txt"
fi

echo "Running secret scan heuristics (git log grep)..."
git log --all --pretty=format:%B | egrep -i --line-number "password|passwd|secret|api[_-]?key|token|aws_access_key_id|aws_secret_access_key|private_key" \
  > "$ARTIFACT_DIR/heuristic-secret-log.txt" || true

if command -v gitleaks >/dev/null 2>&1; then
  echo "Running gitleaks..."
  gitleaks detect --source . --report-path "$ARTIFACT_DIR/gitleaks.json" || true
else
  echo "gitleaks not installed, skipping (heuristic log created)" > "$ARTIFACT_DIR/gitleaks-note.txt"
fi

echo "Attempting to run tests and collect coverage (if a solution file exists)..."
SLN=$(ls *.sln 2>/dev/null | head -n1 || true)
if [ -n "$SLN" ] && command -v dotnet >/dev/null 2>&1; then
  echo "Found solution: $SLN, running tests with coverage collection (may take time)..."
  dotnet test "$SLN" --collect:"XPlat Code Coverage" --logger "trx;LogFileName=day1-results.trx" -v minimal || true
  # Copy coverage files to artifacts (best-effort)
  find . -type f -name "coverage.cobertura.xml" -o -name "coverage.xml" -o -name "*.trx" -maxdepth 6 -print0 2>/dev/null | xargs -0 -I{} bash -c 'cp "{}" "$0"' "$ARTIFACT_DIR" || true
else
  echo "No solution found or dotnet unavailable; skipping tests/coverage" > "$ARTIFACT_DIR/test-note.txt"
fi

echo "Creating summary.txt"
echo "Collected artifacts on $(date)" > "$ARTIFACT_DIR/summary.txt"
echo "Files:" >> "$ARTIFACT_DIR/summary.txt"
ls -la "$ARTIFACT_DIR" >> "$ARTIFACT_DIR/summary.txt" || true

echo "Day 1 collection completed. Review artifacts in $ARTIFACT_DIR and populate findings.csv as needed." 
