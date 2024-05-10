# Contributing

## Set up your dev environment

### Requirements

We use VisualStudio version 13.4.1 for development - that is the version that our linters require.
You must also use `dotnet` 6. You can check your `dotnet` version with:

```bash
dotnet --version
```

`6.0.400`

### Set up

1. Clone the repository
2. `cd` into the repository
3. Open the package and this will install the dependencies

### Build

```bash
dotnet build
```

## Run the linter

```bash
dotnet lint
```

### Unit Tests

```bash
dotnet build
dotnet test --verbosity normal --filter "TestU"
```

### Integration Tests

## Running Tests
For integration tests, we use a `rippled` node in standalone mode to test XRPLSwift code against. To set this up, you can either run `rippled` locally, or set up the Docker container `xrpllabsofficial/xrpld:1.12.0` for this purpose. The latter will require you to [install Docker](https://docs.docker.com/get-docker/).

```bash
# sets up the rippled standalone Docker container - you can skip this step if you already have it set up
docker run -p 6006:6006 --interactive -t --volume $PWD/.ci-config:/config/ xrpllabsofficial/xrpld:1.12.0 -a --start
dotnet build
dotnet test--verbosity normal --filter "TestI"
```

## Generate reference docs

You can see the complete reference documentation at [`XrplCSharp` docs](https://c.xrpl.org). You can also generate them locally using `cd DocFx && docfx`

This updates `docs/` at the top level, where GitHub Pages looks for the docs.

## Update `DefinitionsJson`
Use [this repo](https://github.com/RichardAH/xrpl-codec-gen) to generate a new `DefinitionsJson` file from the rippled source code. Instructions are available in that README.

<!--## Adding and removing packages-->
<!---->
<!--`XRPLSwift` uses `lerna` and `npm`'s workspaces features to manage a monorepo.-->
<!--Adding and removing packages requires a slightly different process than normal-->
<!--as a result.-->

<!--### Adding or removing development dependencies-->
<!---->
<!--`XRPLSwift` strives to use the same development dependencies in all packages.-->
<!--You may add and remove dev dependencies like normal:-->
<!---->
<!--```bash-->
<!--### adding a new dependency-->
<!--npm install --save-dev abbrev-->
<!--### removing a dependency-->
<!--npm uninstall --save-dev abbrev-->
<!--```-->

<!--### Adding or removing runtime dependencies-->
<!---->
<!--You need to specify which package is changing using the `-w` flag:-->
<!---->
<!--```bash-->
<!--### adding a new dependency to `xrpl`-->
<!--npm install abbrev -w xrpl-->
<!--### adding a new dependency to `ripple-keypairs`-->
<!--npm install abbrev -w ripple-keypairs-->
<!--### removing a dependency-->
<!--npm uninstall abbrev -w xrpl-->
<!--```-->

## Release process

### Editing the Code

* Your changes should have unit and/or integration tests.
* Your changes should pass the linter.
* Your code should pass all the tests on Github (which check the linter, unit and integration tests on Swift 5 tests).
* Open a PR against `main` and ensure that all CI passes.
* Get a full code review from one of the maintainers.
* Merge your changes.

### Release

1. Ensure that all tests passed on the last CI that ran on `main`.
___
NOW WE ARE READY TO PUBLISH! No new code changes happen manually now.
___
2. Checkout `main` and `git pull`.
3. Create a new branch to capture updates that take place during this process. `git checkout -b <BRANCH_NAME>`
<!-- 4. Run `npm run docgen` if the docs were modified in this release to update them. -->
<!-- 5. Run `npm run build` to triple check the build still works -->
6. Update the version in the `XrplCSharp.csproj` file and all of the sub packages.
<!-- 6. Run `npx lerna version --no-git-tag-version` - This creates a draft PR and release tags for the new version. -->
<!-- 7. For each changed package, pick what the new version should be. Lerna will bump the versions, commit version bumps to `main`, and create a new git tag for each published package. -->
<!-- 8. Run `npm i` to update the package-lock with the updated versions -->
9. Create a new PR from this branch into `main` and merge it.
10. Checkout `main` and `git pull`
11. Deploy using nuget
<!-- 12. If it asks for it, you may need your [npmjs.com](https://npmjs.com) OTP (one-time password) to complete publication. -->
13. Create a new PR from `main` branch into `nuget_publish` and merge it.
14. `Action` for publish will start automaticaly. Wait for the publication to complete in the `Action` section.
```
the package will be placed in nuget and git.Nuget(git owner @dangell7 )
before push you need to create nuget api key in project settings, if you still not have it)
```
___
NOW YOU HAVE PUBLISHED! But you're not done; we have to notify people!
___
15. Pull the most recent changes to main locally.
16. Run `git tag <tagname> -m <tagname>`, where `<tagname>` is the new package and version (e.g. `0.0.1`), for each version released.
17. Run `git push --follow-tags`, to push the tags to Github.
18. On Github, click the "releases" link on the right-hand side of the page.
19. Click "Draft a new release"
20. Click "Choose a tag", and choose a tag that you just created.
21. Edit the name of the release to match the tag (IE <version\>) and edit the description as you see fit.
22. Repeat steps 19-21 for each release.
23. Send an email to [xrpl-announce](https://groups.google.com/g/xrpl-announce).



## Mailing Lists
We have a low-traffic mailing list for announcements of new `XrplCSharp` releases. (About 1 email every couple of weeks)

+ [Subscribe to xrpl-announce](https://groups.google.com/g/xrpl-announce)

If you're using the XRP Ledger in production, you should run a [rippled server](https://github.com/ripple/rippled) and subscribe to the ripple-server mailing list as well.

+ [Subscribe to ripple-server](https://groups.google.com/g/ripple-server)
