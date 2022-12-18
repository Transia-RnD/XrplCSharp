<!-- # **XrplCSharp**
This package includes the xrpl library. This repository uses a monorepo layout. Please find the README for xrpl [here](http://github.com/transia-RnD/XrplCSharp/).

READMEs for other packages in this monorepo are located at the root of their package, but since newcomers to XRPL are likely to want to use the xrpl package this README is at the root of the project. -->

# **Xrpl APIs overview**

Xrpl provides REST/HTTP and gRPC interfaces.

We strongly recommend that you use our Xrpl client libraries instead of writing your own. Our client libraries include many important features that can be challenging to implement correctly, such as encoding and signing.

If the Xrpl client library for your preferred language does not meet your needs, open a GitHub issue for it instead of writing your own client library.

Supported languages include C#, C++, Java, Swift, Go, Node.js and Python. You should write your own client library only when you use a programming language that is not supported.

## Address Codec

Functions for encoding and decoding XRP Ledger addresses and seeds.

- Address Codec
- Xrpl Codec

## Binary Codec

Functions for encoding objects into the XRP Ledger’s canonical binary format and decoding them.

- Binary Codec
- Binary Wrapper
- Types

## Keypairs

Low-level functions for creating and using cryptographic keys with the XRP Ledger.

- Keypairs
- ED (Algorithm)
- SECP256K1 (Algorithm)

