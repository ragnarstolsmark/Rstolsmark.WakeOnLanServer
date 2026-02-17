# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [4.0.1] - 2026-02-17
### Changed
- Updated Rstolsmark.UnifiClient from 1.0.3 to 1.0.4

## [4.0.0] - 2026-02-17
### Changed
- Added support for configuring Unifi port forwardings with WAN
- Updated to .NET 10
- Updated Azure.Extensions.AspNetCore.Configuration.Secrets from 1.3.1 to 1.4.0
- Updated Azure.Identity from 1.12.0 to 1.17.1
- Updated FluentValidation from 11.9.2 to 12.1.1
- Updated Microsoft.AspNetCore.Owin from 8.0.4 to 10.0.3
- Updated Microsoft.Identity.Web.UI from 2.18.1 to 4.3.0
- Updated Serilog.AspNetCore from 8.0.1 to 10.0.0
- Updated Serilog.Sinks.File from 5.0.0 to 7.0.0
- Updated Serilog.Sinks.Seq from 8.0.0 to 9.0.0
### Removed
- Removed unnecessary System.Net.Ping package

## [3.0.0] - 2024-11-03
### Changed
- Updated to .NET 8
- Updated Nuget packages
- Removed unused FluentValidation client side validation
- Removed dot in error page title
- Added API
- Updated Docker file to use ASP.NET base image

## [2.0.2] - 2022-04-09
### Added
- Version number is now displayed at bottom of page.
### Fixed
- Unifi source and destination port was flipped.
- Unifi port forwardings are now enabled when created.

## [2.0.1] - 2022-03-12
### Changed
- Changed the computer service to be scoped instead of static
### Fixed
- The main grid is reduced to one column on small screens.
- The forward port link is only shown when port forwarding is configured.

## [2.0.0] - 2022-03-06
### Added
- Support for configuring port forwarding with a Unifi controller.
- Authentication and authorization with Azure AD
- Logging to file and console using Serilog
- Configuration secrets storage in Azure keyvault.
- Support for X-Forwarded headers
- Now possible to delete a computer from the wake on lan server
- Editing of computers in wake on lan server configuration

## [1.0.2] - 2019-12-06
### Fixed
- Updated the password authentication library to latest version to protect against timing attacks.

## [1.0.1] - 2019-04-18
### Added
- Initial release of Rstolsmark.WakeOnLanServer with WakeOnLan-functionality.
- Password protection
