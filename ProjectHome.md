![http://lh4.ggpht.com/Wobbit42/SKaW_4a2bJI/AAAAAAAAAkw/Ft5qIws5ucI/NUnitGridRunnerIcon.png](http://lh4.ggpht.com/Wobbit42/SKaW_4a2bJI/AAAAAAAAAkw/Ft5qIws5ucI/NUnitGridRunnerIcon.png)

---

# nLogCruncher #

nLogCrucher is an NLog viewer. Functional but feature lean and without documentation. Works for me.

![http://lh4.ggpht.com/_E5PBVFyg_GA/SxY0otE76BI/AAAAAAAAA08/vrhXnmMktaM/s144/NLogGUI.jpg](http://lh4.ggpht.com/_E5PBVFyg_GA/SxY0otE76BI/AAAAAAAAA08/vrhXnmMktaM/s144/NLogGUI.jpg)

Works (has problems with '|' characters in the log message) with the NLog target:

```
<target name="network" xsi:type="Network" address="udp://127.0.0.2:4000"
	layout="${date:format=HH\:MM\:ss.fff} | ${logger} | ${level} | ${message}"/>
```

Also 'works' (displayed timestamp is 'NQR', seems to be a UTC/local time issue) with:

```
<target name="viewer" xsi:type="NLogViewer" address="udp://127.0.0.2:4000"/>
```


Automatically flush the log from tests:

```
var commandLogger = NLog.LogManager.GetLogger("nLogCruncher.Command");
commandLogger.Info("reset");
```


Features:

  * Dynamic filtering by log level.
  * Dynamic filtering of events by context (logger name) in tree view.
  * Dynamic filtering of events by level.
  * Low CPU loading viewing real time events.
  * Indent messages by context level (right mouse click event context menu item "Show context depth").
  * Show times relative to a selected event (right mouse click event context menu item "Set as reference event").
  * Programmatic log trigger to clear captured logging ("nLogCruncher.Command" context).
  * Exclude context by right mouse click on event.
  * UDP port 4000 listener compatible with NLog Network target.
  * Display tree view of contexts (logger names).

Want list:
  * Context exclusion list by right mouse click on context in tree view.
  * Filter events by message text - regex?.
  * Show matching events count for each level.
  * Show relative time between two selected events.
  * Navigation - got to next/prev event with same level, message & context.
  * Show object state - integration with state engine messaging.
  * Triggers to start/stop logging.
  * Show metrics for selected event, e.g. how often event occurs in the log.
  * Read log file.
  * Allow selected events to be shown always, regardless of context selection.