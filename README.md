# Markdown Note Taking API
https://roadmap.sh/projects/url-shortening-service
## Обзор
Markdown Note Taking API - API, позволяющее пользователям загружать файлы .md (markdown), проверять грамматику, сохранять файл .md и отображать ее в HTML.
## Фичи
* Загрузка и чтение файлов .md
* Отображать в HTML
* Проверка грамотности с помощью Sapling API
## Todo
* Редактирование и сохранение файлов .md

## Технологии
* ASP.NET Core 7.0
* Sapling API

## Требования
* NET 7.0+ SDK
* Ключ Sapling API

## appsettings.json
Переименуйте `appsettings.sample.json` в `appsettings.json`, вставьте свой ключ Sapling API в `SaplingAPIKey` и измените путь к нужной директории, куда будут загружаться .md файлы, в `MarkdownDirectory`

# English


## Overview
Markdown Note Taking API - API that allows users to upload markdown files, check the grammar, save the note, and render it in HTML.
## Features
* Upload and read .md files
* Render .md in HTML
* Check grammer with Sapling API
## To do
* Editing and saving .md files


## Technologies
* ASP.NET Core 7.0
* Sapling API

## Requirements
* NET 7.0+ SDK
* Sapling API key

## appsettings.json
Rename `appsettings.sample.json` to `appsettings.json ` , insert your Sapling API key in `SaplingAPIKey` and change the path to the directory where the .md files will be uploaded in `MarkdownDirectory`.
