﻿// 20210422181233_FixedRssFeeds2.cs
//
// © 2021 Espresso News. All rights reserved.

using Microsoft.EntityFrameworkCore.Migrations;

namespace Espresso.Persistence.EspressoDatabaseMigrations;

/// <inheritdoc/>
public partial class FixedRssFeeds2 : Migration
{
    /// <inheritdoc/>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "NewsPortals",
            keyColumn: "Id",
            keyValue: 115,
            column: "IsEnabled",
            value: true);
    }

    /// <inheritdoc/>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "NewsPortals",
            keyColumn: "Id",
            keyValue: 115,
            column: "IsEnabled",
            value: false);
    }
}
