# xrpl.c Release Process

## Cutting a Release

The full process for cutting a release is as follows:

0. Checkout a new branch:
   `git checkout -b v1.0.0` # v1.0.0-release

1. Change the version in the sln/nuget file:
   `v1.0.0`

2. Add, and commit the changes, push up the branch, and open a PR:
   `git add .`
   `git commit -m 'RELEASE v1.0.0'`
   `git push --set-upstream origin HEAD`

3. Open PR request

   ``

4. Once the PR is merged, checkout the `main` branch:
   `git checkout main`

5. Delete `main` branch (Optional):
   `git branch -d v1.0.0`

6. Make a new Git tag that matches the new version (make sure it is associated with the right commit SHA): FIXUP
   `git tag -a v1.0.0 -m "v1.0.0"`

7. Push up the tag from `main`:
   `git push origin v1.0.0`