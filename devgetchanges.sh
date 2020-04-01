#!/bin/bash

cd ~

rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/src/app/plugins/modules/monitoring-pn

cp -a Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/monitoring-pn Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/src/app/plugins/modules/monitoring-pn

rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn

cp -a Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/Monitoring.Pn Documents/workspace/microting/eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn

# Test files rm
rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Tests/monitoring-settings/
rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Tests/monitoring-general/
rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Page\ objects/Monitoring/
rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/wdio-headless-plugin-step2.conf.js

# Test files cp
cp -a Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/monitoring-settings Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Tests/monitoring-settings
cp -a Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Tests/monitoring-general Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Tests/monitoring-general
cp -a Documents/workspace/microting/eform-angular-frontend/eform-client/e2e/Page\ objects/Monitoring Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/e2e/Page\ objects/Monitoring
cp -a Documents/workspace/microting/eform-angular-frontend/eform-client/wdio-plugin-step2.conf.js Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/wdio-headless-plugin-step2.conf.js
