import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  OperationDataResult,
  OperationResult,
} from 'src/app/common/models/operation.models';
import {
  NotificationRuleModel,
  NotificationRulesListModel,
  NotificationsRequestModel,
} from '../models';
import { ApiBaseService } from 'src/app/common/services';

export let MonitoringPnNotificationRulesMethods = {
  Base: 'api/monitoring-pn/rules',
};
@Injectable({
  providedIn: 'root',
})
export class MonitoringPnNotificationRulesService {
  constructor(private apiBaseService: ApiBaseService) {}

  getRulesList(
    model: NotificationsRequestModel
  ): Observable<OperationDataResult<NotificationRulesListModel>> {
    return this.apiBaseService.get(
      MonitoringPnNotificationRulesMethods.Base,
      model
    );
  }

  getRule(id: number): Observable<OperationDataResult<NotificationRuleModel>> {
    return this.apiBaseService.get(
      MonitoringPnNotificationRulesMethods.Base + '/' + id
    );
  }

  updateRule(model: NotificationRuleModel): Observable<OperationResult> {
    return this.apiBaseService.put(
      MonitoringPnNotificationRulesMethods.Base,
      model
    );
  }

  createRule(model: NotificationRuleModel): Observable<OperationResult> {
    return this.apiBaseService.post(
      MonitoringPnNotificationRulesMethods.Base,
      model
    );
  }

  deleteRule(id: number): Observable<OperationResult> {
    return this.apiBaseService.delete(
      MonitoringPnNotificationRulesMethods.Base + '/' + id
    );
  }
}
