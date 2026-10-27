# File Handling

Upload, download, image handling, QR generation and report export through an ASP.NET Core API.

## Stack

**Backend** ASP.NET Core Web API · QRCoder · QuestPDF · ClosedXML · Swagger

## What it does

| Endpoint group | What it handles |
|---|---|
| `ImagesController` | Upload and download of image files, served from `Uploads/` |
| `QrController` | QR code generation via QRCoder |
| `ReportsController` | Report export — PDF with QuestPDF, Excel with ClosedXML |

Between them these cover the three cases that come up most with file work: taking a file
in, generating one on the fly, and rendering structured data into a document a user can
open.

## Layout

```
file-handling/
├─ backend/     .NET Web API (FileHandlingApi) — Controllers/, Uploads/
└─ frontend/    Angular scaffold — config files only, no src/
```

The backend works. The frontend has `angular.json`, `package.json` and the tsconfigs but
**no `src/` folder**, so it will not serve as-is — it needs re-scaffolding.

## Run

```bash
# backend  →  https://localhost:7292
cd file-handling/backend && dotnet run
```

Swagger UI is served at the root in development — the easiest way to try the uploads.

> Port `7292` collides with `user-management`'s UserManagement.Day19. Change one of them
> in `Properties/launchSettings.json` before running both.
