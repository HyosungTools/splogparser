# Release Process

This document describes how to perform a release of `splogparser`.

## 🚧 Workflow Summary

- **Pre-releases**: Automatic on every push to `main`
- **Releases**: Manual or scheduled once per month (uses semantic versioning)

## 🔁 Release Steps

1. Go to **GitHub > Actions > Manual Release**
2. Click "Run workflow"
3. The workflow will:
   - Analyze commits
   - Auto-generate new version
   - Update `VERSION` and `Version.cs`
   - Generate `CHANGELOG.md`
   - Create a GitHub Release

## 📄 Files Updated

- `VERSION`: plain text version string
- `Version.cs`: embedded in compiled app
- `CHANGELOG.md`: Git-based change log

## 📌 Requirements

- Commit messages must follow [Conventional Commits](./ConventionalCommits.md)

## 🔒 Permissions

You must have push access and GitHub Actions permissions to run this workflow.
