# Manual Verification Checklist

The direct prefab edits replace the runtime Build postfix. Validate the following when testing the
mod in-game:

- Place a checkpoint: the Suit Marker should default to "Vacancy Only" without additional clicks.
- Construct a manual access door that does not displace gas: it should open automatically a moment
  after construction and stay open across save/load.
- Build a smart reservoir: its activation and deactivation sliders should match the configured
  percentages immediately after placement.
- Build a generator with a delivery threshold (e.g. Coal Generator): confirm the slider defaults to
  the configured value, ensuring the LoadGeneratedBuildings initialization still runs.

Re-run the checklist after toggling any relevant options to confirm the prefab defaults respect the
configuration.
