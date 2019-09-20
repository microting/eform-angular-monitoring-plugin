import {Router} from '@angular/router';

import {Component, OnInit} from '@angular/core';
import {MonitoringBaseSettingsModel} from '../../models/monitoring-base-settings.model';
import {MonitoringPnSettingsService} from '../../services';

@Component({
  selector: 'app-monitoring-settings',
  templateUrl: './monitoring-settings.component.html',
  styleUrls: ['./monitoring-settings.component.scss']
})
export class MonitoringSettingsComponent implements OnInit {
  spinnerStatus = false;
  settingsModel: MonitoringBaseSettingsModel = new MonitoringBaseSettingsModel();

  constructor(private monitoringPnSettingsService: MonitoringPnSettingsService, private router: Router) {
  }

  ngOnInit() {
    this.getSettings();
  }


  getSettings() {
    // debugger;
    this.spinnerStatus = true;
    this.monitoringPnSettingsService.getAllSettings().subscribe((data) => {
      if (data && data.success) {
        this.settingsModel = data.model;
      } this.spinnerStatus = false;
    });
  }

  updateSettings() {
    this.spinnerStatus = true;
    this.monitoringPnSettingsService.updateSettings(this.settingsModel)
      .subscribe((data) => {
        if (data && data.success) {

        } this.spinnerStatus = false;
      });
  }
}
