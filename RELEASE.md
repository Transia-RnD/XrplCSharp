# Xrpl C# Release Process

## Cutting a Release

The full process for cutting a release is as follows:

0. Checkout a new branch:
   `git checkout -b 1.0.0` # 1.0.0-release

1. Python / Pip Bumpversion

   `pip3 install bumpversion`

   `bumpversion --current-version 1.0.0 minor setup.py xumm/__init__.py`

2. Change the version in the setup.py file:
   `VERSION = "1.0.0"`

3. Add, and commit the changes, push up the branch, and open a PR:
   `git add .`
   `git commit -m 'RELEASE 1.0.0'`
   `git push --set-upstream origin HEAD`

4. Open PR request

   ``

5. Once the PR is merged, checkout the `main` branch:
   `git checkout main`

6. Delete `main` branch (Optional):
   `git branch -d 1.0.0`

7. Make a new Git tag that matches the new version (make sure it is associated with the right commit SHA): FIXUP
   `git tag -a 1.0.0 -m "cut 1.0.0"`

8. Push up the tag from `main`:
   `git push origin 1.0.0`