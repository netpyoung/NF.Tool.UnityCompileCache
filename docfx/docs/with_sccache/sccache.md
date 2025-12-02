# sccache

- https://github.com/mozilla/sccache
  - https://github.com/mozilla/sccache/releases/
  - [Architecture.md](https://github.com/mozilla/sccache/blob/main/docs/Architecture.md)
  - [Caching.md](https://github.com/mozilla/sccache/blob/main/docs/Caching.md)
  - [Configuration.md](https://github.com/mozilla/sccache/blob/main/docs/Configuration.md)
  - [Configuration.md#misc](https://github.com/mozilla/sccache/blob/main/docs/Configuration.md#misc)
  - [Xcode.md](https://github.com/mozilla/sccache/blob/main/docs/Xcode.md)

|         | config file path                                     |
| ------- | ---------------------------------------------------- |
| Windows | %APPDATA%\Mozilla\sccache\config\config              |
| macOS   | ~/Library/Application Support/Mozilla.sccache/config |
| Linux   | ~/.config/sccache/config                             |

- %APPDATA%\Mozilla\sccache\config\config
- C:\Users\{uername}\AppData\Roaming\Mozilla\sccache\config\config

``` toml
[cache.redis]
url = "redis://127.0.0.1:6379"
```

``` cmd
> sscache --show-stats
Compile requests                    847
Compile requests executed           847
Cache hits                          846
Cache hits (C/C++)                  846
Cache misses                          1
Cache misses (C/C++)                  1
Cache hits rate                   99.88 %
Cache hits rate (C/C++)           99.88 %
...
```