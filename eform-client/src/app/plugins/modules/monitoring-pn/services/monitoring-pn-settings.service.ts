import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  OperationDataResult,
  OperationResult,
} from '../../../../common/models';
import { MonitoringBaseSettingsModel } from '../models';
import { ApiBaseService } from 'src/app/common/services';

export let MonitoringSettingsMethods = {
  MonitoringSettings: 'api/monitoring-pn/settings',
};
@Injectable()
export class MonitoringPnSettingsService {
  constructor(private apiBaseService: ApiBaseService) {}

  getAllSettings(): Observable<
    OperationDataResult<MonitoringBaseSettingsModel>
  > {
    return this.apiBaseService.get(
      MonitoringSettingsMethods.MonitoringSettings
    );
  }

  updateSettings(
    model: MonitoringBaseSettingsModel
  ): Observable<OperationResult> {
    return this.apiBaseService.post(
      MonitoringSettingsMethods.MonitoringSettings,
      model
    );
  }
}
