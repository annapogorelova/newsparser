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

echo "--- git pull ---"
git pull
echo ""

# Assuming that we are in /var/www/newsparser
PROJECT_PATH=$CURRENT_DIR/../backend/newsparser.$PROJECT_NAME/newsparser.$PROJECT_NAME.csproj
OUTPUT_PATH=$CURRENT_DIR/../backend/dist/$PROJECT_NAME

rm -rf $OUTPUT_PATH

echo "--- dotnet publish ---"
dotnet publish $PROJECT_PATH -o=$OUTPUT_PATH -c=Production
echo ""

if [[ "$PROJECT_NAME" == "web" ]];
	then
		echo "--- generating api docs ---"
		apidoc -i $CURRENT_DIR/../backend/newsparser.$PROJECT_NAME/ -o $CURRENT_DIR/../docs
fi

echo "--- restarting" $SERVICE_NAME "---"
systemctl restart $SERVICE_NAME
systemctl status $SERVICE_NAME