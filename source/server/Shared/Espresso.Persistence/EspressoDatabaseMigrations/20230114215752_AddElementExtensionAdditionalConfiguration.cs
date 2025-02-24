﻿// 20230114215752_AddElementExtensionAdditionalConfiguration.cs
//
// © 2022 Espresso News. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Espresso.Persistence.EspressoDatabaseMigrations;

/// <inheritdoc />
public partial class AddElementExtensionAdditionalConfiguration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<int>(
            name: "ImageUrlParseConfiguration_ElementExtensionValueParseType",
            table: "RssFeeds",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        _ = migrationBuilder.AddColumn<int>(
            name: "ImageUrlParseConfiguration_ElementExtensionValueType",
            table: "RssFeeds",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "ImageUrlParseConfiguration_ElementExtensionValueParseType",
            table: "RssFeeds");

        _ = migrationBuilder.DropColumn(
            name: "ImageUrlParseConfiguration_ElementExtensionValueType",
            table: "RssFeeds");
    }
}
