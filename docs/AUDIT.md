# Definition Plugin Audit — September 25, 2026

This audit covers settings, caching, lookups, cancellation, external actions, and the release workflow. Priorities reflect user impact.

| Priority | Finding and reproduction | Impact | Status |
| --- | --- | --- | --- |
| High | Set `CacheMaxSize=0` in PowerToys or `config.json`, then perform a lookup that returns a result. | Inserting into the LRU cache attempts to remove an empty node and fails. | Fixed: settings accept only positive values; invalid file values are normalized at load time (runtime only — the file itself is not rewritten until settings are saved). |
| High | Set `HttpTimeoutSeconds=0` or a negative value. | Lookup fails because the timeout is invalid. | Fixed: settings accept only positive values; invalid file values are normalized at load time (runtime only). |
| High | Start a slow lookup, then enter a new query. | Providers could turn cancellation into an empty result and display “not found”; an obsolete result could enter the cache. | Fixed: capture and propagate the cancellation token, then check it before caching. |
| High | Return a `file:` source or audio URL from an API and activate it. | The shell or media player could open a local resource. | Fixed: only `http` and `https` URLs are accepted, including at action time. |
| Medium | Set `LatinLanguages="en"` in `config.json`. | Loading the file replaced this choice with `en,fr,it`. | Fixed: an explicit English-only choice is preserved. |
| Medium | Change display settings, cache size, timeout, or the Datamuse key while the plugin is running. | Cached results could reflect old settings; the HTTP client and suggestion provider retained old values. | Fixed: configuration changes reset the cache; the timeout applies per lookup; the key is read per request. |
| Medium | Create two `Main` instances, dispose one, then search with the other. | The shared `HttpClient` had already been disposed. | Fixed: the shared client remains available for the process lifetime. |
| Medium | Open a pull request with a build or test failure. | The release workflow checked the build only after a tag was created. | Fixed: Windows x64 tests run on pull requests and before release builds. |

## Deferred

- `Task.Run` followed by synchronous waiting in `Main.Query` complicates the lookup path. Change it separately after confirming the `IDelayedExecutionPlugin` contract and behavior in PowerToys.
- The LRU cache and cancellation source have no shared synchronization for concurrent `Query` calls. If PowerToys calls them concurrently, add a test for that scenario and protect the shared state.

## Verification

`dotnet build Definition/Definition.sln -p:Platform=x64 -p:EnableWindowsTargeting=true` passed with .NET 9. Local `dotnet test` could not run the Windows tests on Linux because VSTest IPC was restricted. Windows CI runs them for pull requests and release tags.
