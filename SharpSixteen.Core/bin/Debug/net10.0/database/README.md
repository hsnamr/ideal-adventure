# Database JSON files

Optional data files loaded at startup. Place these in the `database/` folder next to the game executable (or set path in code). If a file is missing or invalid, built-in data is used.

- **items.json** – Extra items (Id, Name, Kind, Effect, Value)
- **skills.json** – Extra skills (Id, Name, MpCost, Power, Target)
- **events.json** – Extra events (Id, Trigger, Text, PortraitId, NextEventId)
- **enemies.json** – Extra enemies (Id, Name, SpriteId, Hp, Attack, Experience)

Format: JSON array of objects. Property names are case-insensitive.
