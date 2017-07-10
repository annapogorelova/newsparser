#!/bin/bash
#set -e

CURRENT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

if [ -z "$1" ]
    then
        echo "No argument supplied"
        exit
fi

PROJECT_NAME=$1
SERVICE_NAME=newsparser-$PROJECT_NAME.service

# Assuming the service exists
echo "---" $SERVICE_NAME "status ---"
systemctl is-active $SERVICE_NAME
if [[ "$?" -eq "active" ]];
    then
        systemctl stop $SERVICE_NAME
fi
echo ""

echo "--- git pull ---"
git pull
echo ""

# Assuming that we are in /var/www/newsparser
PROJECT_PATH=$CURRENT_DIR/../backend/newsparser.$PROJECT_NAME/newsparser.$PROJECT_NAME.csproj
OUTPUT_PATH=$CURRENT_DIR/../dist/backend/$PROJECT_NAME

rm -rf $OUTPUT_PATH

echo "--- dotnet publish ---"
dotnet publish $PROJECT_PATH -o=$OUTPUT_PATH -c=Production
echo ""

if [[ "$PROJECT_NAME" == "web" ]];
	then
		echo "--- generating api docs ---"
		apidoc -i $CURRENT_DIR/../backend/newsparser.web/ -o ./docs
fi

echo "--- starting" $SERVICE_NAME "---"
systemctl start $SERVICE_NAME
systemctl status $SERVICE_NAME