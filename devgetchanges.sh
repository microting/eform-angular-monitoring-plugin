#!/bin/bash

cd ~

if [ -d "Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/src/app/plugins/modules/monitoring-pn" ]; then
	rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/src/app/plugins/modules/monitoring-pn
fi

cp -av Documents/workspace/microting/eform-angular-frontend/eform-client/src/app/plugins/modules/monitoring-pn Documents/workspace/microting/eform-angular-monitoring-plugin/eform-client/src/app/plugins/modules/monitoring-pn

if [ -d "Documents/workspace/microting/eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn" ]; then
	rm -fR Documents/workspace/microting/eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn
fi

cp -av Documents/workspace/microting/eform-angular-frontend/eFormAPI/Plugins/Monitoring.Pn Documents/workspace/microting/eform-angular-monitoring-plugin/eFormAPI/Plugins/Monitoring.Pn
