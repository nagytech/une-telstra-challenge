#!/bin/bash
set -e

echo 'Purging web-server and web-client'
rm -rf /var/www/html/*.*

echo 'Publishing web server'
cp -R src/web-server/api/ /var/www/html/

echo 'Publishing web client'
cp -R src/web-client/* /var/www/html/

echo 'Displaying release'
tree /var/www/html

echo 'Done. Have a nice day'
