# File Handling

File upload and download through a .NET Web API.

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

> Port `7292` collides with `user-management`'s UserManagement.Day19. Change one of them
> in `Properties/launchSettings.json` before running both.
