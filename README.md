# FillTheFridge
Køleskabstetris

CONTRIBUTING:

Create a branch for your new feature.

Make your changes.

    Avoid making changes to more files than necessary for your feature (i.e. refrain from combining your "real" pull request with incidental bug fixes). This will simplify the merging process and make your changes clearer.
    Very much avoid making changes to the Unity-specific files, like the scene and the project settings unless absolutely necessary. Changes here are very likely to cause difficult to merge conflicts. Work in code as much as possible. (We will be trying to change the UI to be more code-driven in the future.) Making changes to prefabs should generally be safe -- but create a copy of the main scene and work there instead (then delete your copy of the scene before committing).

Commit your changes. It "saves" them.

While you were working some other pull request might have gone in the breaks your stuff or vice versa. This can be a merge conflict but also conflicting game logic or code. Before you test, update your master, and merge with your with it.

Test. Start the game and do something related to your feature/fix. Does it still work?

If so, push the branch, uploading it to Github.

Make a "Pull Request" from your branch here on Github.

