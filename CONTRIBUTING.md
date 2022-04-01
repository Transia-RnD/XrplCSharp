# Contributing

## Release process

### Editing the Code

* Your changes should have unit and/or integration tests.
* Your changes should pass the linter.
* Your code should pass all the unit tests on Github (which check all 3 versions of Python).
* Open a PR against `master` and ensure that all CI passes.
* Get a full code review from one of the maintainers.
* Merge your changes.

### Release

1. Run integration tests on `master`, using [Github Actions](https://github.com/XRPLF/xrpl.c/actions/workflows/integration_test.yml), which runs them on all 3 versions of Python.
2. Create a branch off master that properly increments the version in `Xrpl.C.csproj` and updates the `CHANGELOG` appropriately. We follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html).
3. Merge this branch into `master`.
4. Run integration tests on `master` again just in case.
5. Create a new Github release/tag off of this branch.
6. Locally build and download the package.
    1. Pull master locally.
    2. Run `poetry build` to build the package locally.
    3. Locally download the package by running `pip install path/to/local/xrpl.c/dist/.whl`.
    4. Make sure that this local installation works as intended, and that changes are reflected properly.
7. Run `poetry publish --dry-run` and make sure everything looks good.
8. Publish the update by running `poetry publish`.
    * This will require entering Nuget login info.
9. Send an email to [xrpl-announce](https://groups.google.com/g/xrpl-announce).

## Mailing Lists
We have a low-traffic mailing list for announcements of new `xrpl.c` releases. (About 1 email every couple of weeks)

+ [Subscribe to xrpl-announce](https://groups.google.com/g/xrpl-announce)

If you're using the XRP Ledger in production, you should run a [rippled server](https://github.com/ripple/rippled) and subscribe to the ripple-server mailing list as well.

+ [Subscribe to ripple-server](https://groups.google.com/g/ripple-server)
