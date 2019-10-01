import {Component, OnInit} from '@angular/core';
import {MonitoringPnSettingsService} from '../../services';
import {MonitoringBaseSettingsModel} from '../../models';

@Component({
  selector: 'app-monitoring-settings',
  templateUrl: './monitoring-settings.component.html',
  styleUrls: ['./monitoring-settings.component.scss']
})
export class MonitoringSettingsComponent implements OnInit {
  spinnerStatus = false;
  settingsModel: MonitoringBaseSettingsModel = new MonitoringBaseSettingsModel();

  constructor(private monitoringPnSettingsService: MonitoringPnSettingsService) {
  }

  ngOnInit() {
    this.getSettings();
  }


  getSettings() {
    this.spinnerStatus = true;
    this.monitoringPnSettingsService.getAllSettings().subscribe((data) => {
      if (data && data.success) {
        this.settingsModel = data.model;
      }
      this.spinnerStatus = false;
    });
  }

  updateSettings() {
    this.spinnerStatus = true;
    this.monitoringPnSettingsService.updateSettings(this.settingsModel)
      .subscribe(() => {
        this.spinnerStatus = false;
      });
  }
}
