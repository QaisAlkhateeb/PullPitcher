# PullPitcher Bot

## Overview
The PullPitcher Bot is designed to facilitate the review process of pull requests by assigning reviewers and tracking the assignments. It is integrated with Microsoft Teams and provides several commands to manage and interact with pull request assignments. This bot is currently in the development phase and may undergo significant changes before its final release.

## Features
- **Set Catchers**: Assigns reviewers to repositories based on provided email addresses.
- **List Catchers**: Lists all the reviewers assigned to a specific repository.
- **Pitch**: Automatically assigns a pull request to a reviewer, ensuring the reviewer is not the owner of the pull request.
- **History**: Displays the history of pull request assignments.
- **Me**: Lists all pull requests assigned to the user issuing the command.

## Commands

### Set Catchers
- **Syntax**: `SetCatchers "repoKey" "email1,email2,..."`
- **Description**: Updates the list of reviewers for a specific repository. If no emails are provided, all team members are added as reviewers.
- **Example**: `SetCatchers "/orgName/projectName/repoName" "user1@example.com,user2@example.com"`

### List Catchers
- **Syntax**: `ListCatchers "/orgName/projectName/repoName"`
- **Description**: Lists all reviewers assigned to the specified repository.
- **Example**: `ListCatchers "/orgName/projectName/repoName"`

### Pitch
- **Syntax**: Automatically triggered when a pull request URL is detected in a message.
- **Description**: Assigns the pull request to an appropriate reviewer who is not the owner.
- **Example**: Just paste the URL of the pull request in the chat.

### History
- **Syntax**: `History [number]`
- **Description**: Shows the history of the last `number` pull request assignments. If no number is provided, it shows all history.
- **Example**: `History 10`

### Me
- **Syntax**: `Me`
- **Description**: Lists all pull requests that have been assigned to the user issuing the command.
- **Example**: `Me`

## Development Note
Please note that this bot is still in development. Features and commands are subject to change. Feedback and contributions are welcome to improve functionality and user experience.

## Icons Linces

[Gitlab icons created by Grey Zundapp - Flaticon](https://www.flaticon.com/free-icons/gitlab)
