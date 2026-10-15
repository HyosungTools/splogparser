# Pull Request Process

This document describes how to get a change into `main`. It sits between [How To Build](./HowToBuild.md) (getting a working build locally) and the [Release Process](./ReleaseProcess.md) (shipping what's on `main`).

Every change reaches `main` through a pull request. The PR title and label you choose here **become the release notes**, so they matter more than they look.

## 🔁 Workflow Summary

```
issue → branch from origin/main → commit → build & test locally → push → PR (title + label) → CI green → squash merge → prerelease published
```

## 1. Start from an issue

Open (or pick) a GitHub issue describing the problem or feature. From the issue page, use **Development > Create a branch**. GitHub names the branch `<issue#>-<issue-title>`, e.g. `278-ipm-mediadata`, which keeps the branch, issue and PR linked.

## 2. Cut the branch fresh from `origin/main`

Always start from the latest `main`. Never reuse an old branch or a long-lived maintenance branch.

```text
git fetch origin
git switch -c 278-ipm-mediadata --no-track origin/main
```

`--no-track` stops the new branch from tracking `origin/main`, so `git push` goes to your branch rather than trying to push to main.

If you created the branch on GitHub, just fetch and check it out:

```text
git fetch origin
git switch 278-ipm-mediadata
```

## 3. Make the change

- Keep commits focused: two or three commits per feature, each with a message that explains what and why.
- For parsing bugs, add a unit test built from the failing log line **first**, then fix the code.
- Follow the existing patterns (View / Table / XML / XSD, enum names match class names, etc.).

## 4. Build and test locally

Follow [How To Build](./HowToBuild.md). Both the build and the unit tests must pass before you push; CI runs the same commands.

## 5. Check you're only committing source

No `bin\`, `obj\`, `.vs\` or `dist\` content should be tracked:

```text
git status
git ls-files | findstr /i "\\bin\\ \\obj\\ \\.vs\\ dist\\"
```

The `findstr` command should print nothing.

## 6. Push and open the PR

```text
git push -u origin 278-ipm-mediadata
```

GitHub shows a **Compare & pull request** banner. Base branch is `main`. The description is pre-filled from `.github/pull_request_template.md` with a checklist covering the steps below.

### Title — write it as the release note

The PR title is copied verbatim into the release notes. Write it for the support team, not for developers:

| ❌ Avoid | ✅ Prefer |
|---------|----------|
| `278 ipm mediadata` (the default branch name) | `IPM: show media data events in OverSummary` |
| `Remove WriteExcel from OverTable` | *(fine as-is if labelled `internal` — see below)* |
| `fix stuff` | `DispView: fix dispense parsing for v25 logs` |

Guidelines:

- Lead with the View or worksheet affected (`XmlParam:`, `CashTrailView:`, `OverSummary:`).
- Say what the user will see differently, in one sentence.
- If the PR does two unrelated things, it should probably be two PRs.

### Label — decides the release notes section

Pick **one** label from this table. The mapping lives in `.github/release.yml`.

| Label | Release notes section | Use for |
|-------|----------------------|---------|
| `new-view` | 🆕 New Views & Worksheets | A new View plugin or a new worksheet |
| `enhancement` | ✨ Improvements | New information or better presentation in an existing View |
| `bug` or `parsing` | 🐛 Parsing Fixes | Lines that were mis-parsed, missed, or showed wrong values |
| `data` | 📋 Config / Known Issues Data | Changes to XML seed data, Messages tables, rules files |
| `internal` | *(hidden)* | Refactoring, cleanup, no user-visible change |
| `ci` | *(hidden)* | Build, workflow, installer, versioning changes |
| *(none)* | Other Changes | Avoid — means nobody chose |

### Description

- Link the issue with `Closes #278` so it closes on merge.
- Name the support ticket or log archive that motivated the change (e.g. `NHSWS-17121`), so the fix can be traced back to a real case.
- Briefly say what changed and how you tested it (which log archive, which worksheet to look at).
- Screenshots of the affected worksheet are welcome.

## 7. Wait for CI

The `build` workflow runs on every PR: it builds, runs the unit tests, and builds the installer. PR builds don't publish anything. Don't merge on a red build.

## 8. Squash merge and delete the branch

- Use **Squash and merge**. Check the squash commit message before confirming. By default it's the PR title plus the commit list.
- Click **Delete branch** after merging.
- Locally, clean up and go back to `main`:

```text
git switch main
git pull
git branch -d 278-ipm-mediadata
```

## 9. What happens next

Merging to `main` triggers the `build` workflow, which publishes a **prerelease** (e.g. `v8.4.1-prerelease.1`) with auto-generated notes. Your PR appears in it under the section chosen by its label.

The next manual release collects every PR merged since the previous full release. See [Release Process](./ReleaseProcess.md).

## 🛠 Fixing a PR after merge

Titles and labels can still be changed after merging. Edit the PR on GitHub, then either regenerate or hand-edit the notes on the affected release. It's easiest to tidy these up just before running a manual release.

## 📌 One-time setup

`.github/release.yml` refers to labels that must exist in the repository. Make sure these are created under **Issues > Labels**: `new-view`, `enhancement`, `bug`, `parsing`, `data`, `internal`, `ci`.
