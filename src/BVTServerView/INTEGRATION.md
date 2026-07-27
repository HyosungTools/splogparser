# BVTServerView — BlueVerse Teller server allocation ledger + fault summary (AV)

A splogparser view over the BlueVerse Teller (ActiveTeller) **server** logs. Where AVView dumps one
categorized row per line, BVTServerView reconstructs two things a support engineer actually reads:

- **Allocation** — one row per teller session request: `time · asset · request · rule · assigned? ·
  teller · waitsec`. The server-side answer to "was a teller available, who got it, how long was
  the wait" — and it flags requests that were never assigned.
- **ServerFaults** — server exceptions **grouped by signature** (digits, GUIDs and URIs normalized),
  with `count` and first/last seen. A multi-thousand-line exception storm becomes a handful of lines.

It rides the existing `-v` (AV) handler — no `Program.cs` change.

## Install (same as CallView / TxnView)

1. Copy this `BVTServerView` folder into `src\` next to `AVView`.
2. Solution Explorer → Add → Existing Project → `src\BVTServerView\BVTServerView.csproj`.
3. Build **Release** (output goes to `dist` via `Directory.Build.props`).
4. Run:  `splogparser.exe -v BVTServer -f <zip containing ActiveTellerServer*.log>`
   (`-v *` runs AVView and BVTServerView together.)

## Verified against real server logs (2026-07-22)

The fault summary collapsed the storm to signatures, e.g.:

```
4658x  Schedule manager processor exception: Thread was being aborted
  10x  Unexpected exception authorizing request Uri <uri> - client disconnected
   8x  FiservESF: ERROR Deserializing (token = HTML, not JSON)  -> customer ID lookup down
   2x  DB update exception - INSERT conflicted with FOREIGN KEY constraint FK_dbo.T#
```

Allocation reconstructed one request (asset KS000643, PREFER_BRANCH, assigned) — a quiet
single-teller capture; the ledger is richer on a busy multi-teller server.

## Security note

The existing AVView copies the raw Startup settings payload into its worksheet, which includes
`<ClientCredentialsPassword>` in **plaintext**. BVTServerView deliberately emits only allocations and
fault signatures, so no credential reaches its output. Worth masking that field in AVView too.

## Next iteration

The server logs share `client session` / `asset` / teller IDs with the workstation (CallView) and
ATM (Over/Txn) logs — so this view is the third vantage point that makes true end-to-end
correlation ("customer requested → server assigned teller X → workstation answered → call dropped")
reachable.
