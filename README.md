# AR Treasure Hunt

Augmented-reality object detection game made using Unity3D & Vuforia.

## Workflow

---

1. Open app & login (can be dummy login)
2. Once logged in, will receive text clue from database.
3. With clue, search for:

	* image targets (for 1st pass)
	* object tracked item (e.g. bowling trophy)
	* gps-locked location (constructing - partially implemented)

4. Once item found:

	* message is sent to db (where item is checked off)
	* then, next clue is delivered.