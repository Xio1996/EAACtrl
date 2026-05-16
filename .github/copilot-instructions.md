# Copilot Instructions

## Project Guidelines
- ASCOM.Astrometry.AstroUtils.AstroUtils provides a DeltaT() method; use AstroUtils.DeltaT() when converting UTC Julian date to Terrestrial Time (TT) instead of a local estimator.
- ASCOM.Utilities.Util does not provide a DeltaT property; compute Delta‑T (TT−UTC) with a local estimator when converting UTC Julian date to Terrestrial Time (TT).