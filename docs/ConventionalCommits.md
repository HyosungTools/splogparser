# Commit Style Guide (Conventional Commits)

Use these prefixes in commit messages to enable automated versioning and changelogs.

## ✅ Types

- `feat:` — New feature (minor bump)
- `fix:` — Bug fix (patch bump)
- `chore:` — Build/tools/infra changes
- `docs:` — Documentation only
- `refactor:` — Non-functional changes
- `break:` — Breaking change (major bump)

## 💡 Examples

```
feat: add CSV export option
fix: handle null in parser
docs: update install instructions
break: remove legacy ZIP parser
```

## 🔁 Tip

Run `git commit -m "feat: support SIU retry logic"` to stay compliant.
