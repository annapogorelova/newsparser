#!/bin/bash
#set -e

if [ -z "$1" ]
    then
        echo "No argument supplied"
        exit
fi

SERVICE_NAME=newsparser-$1.service

# Assuming the service exists
echo "---" $SERVICE_NAME "status ---"
systemctl is-active $SERVICE_NAME
if [[ "$?" -eq "active" ]];
    then
        systemctl stop $SERVICE_NAME
fi
echo ""

cd ..
echo "--- git pull ---"
git pull
echo ""

# Assuming that we are in /var/www/newsparser
PROJECT_PATH=./backend/newsparser.$1/newsparser.$1.csproj
OUTPUT_PATH=./dist/backend/$1

echo "--- dotnet publish ---"
dotnet publish $PROJECT_PATH -o=$OUTPUT_PATH -c=Production
echo ""

echo "--- starting" $SERVICE_NAME "---"
systemctl start $SERVICE_NAME
systemctl status $SERVICE_NAME