# Societies Stranding

Planets are running out of room. You fly a ship that tows population pods from crowded planets to ones with space left, and try to get a high score before every planet fills up.

- Play: [itch.io](https://unitedfailures.itch.io/societies-stranding)
- Made: October 2023 for Ludum Dare 54
- Team: [@mfclinton](https://github.com/mfclinton) (programming), [@DavidKoleczek](https://github.com/DavidKoleczek) (programming), [@MrAozora](https://github.com/MrAozora) (music), [erasound](https://erasound.itch.io) (sound effects)
- Credits: circular buffer from joaoportela's [CircularBuffer-CSharp](https://github.com/joaoportela/CircularBuffer-CSharp)
- Engine: Unity, C#

This is an export of a private repo with only the code we wrote. Art, audio, the Unity project files, and third party plugins aren't included. The history is squashed into one commit.

## What I built

- Every planet pulls on the ship and the pods with inverse square gravity, calculated in parallel with Unity's Job System. Anything that drifts close to a planet gets eased into a steady circular orbit.
- Towed pods follow the ship like a train. The ship tracks its recent path, and each pod springs toward a point further back along it. Pods snap loose if they fall too far behind, and letting go flings them with the ship's momentum.
- Planets develop through stages at random times, and once a planet is fully developed it starts launching pods of people. Each pod has to reach a planet with room before it expires.
- The minimap is centered on the sun and scaled to fit the whole solar system. It shows the ship pointing the way it's facing, every planet, the pods as dots, and a marker on planets that are launching pods.
