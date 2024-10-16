#!/bin/bash

# packages
apt-get update -y
apt-get upgrade -y

apt-get install -y git
apt-get install -y vim

# git
git config --global --add safe.directory /workspaces/satel-over-dotnet
git config --global user.name "$GIT_USER_NAME"
git config --global user.email "$GIT_USER_EMAIL"
