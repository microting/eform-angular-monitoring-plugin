import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';

import { Observable} from 'rxjs';
import {Router} from '@angular/router';
import {OperationDataResult, OperationResult} from 'src/app/common/models/operation.models';
import {BaseService} from 'src/app/common/services/base.service';

import {
  MonitoringNotificationRuleModel,
  MonitoringNotificationRulesListModel,
  MonitoringNotificationsRequestModel
} from '../models';

export let MonitoringPnNotificationRulesMethods = {
  Base: 'api/monitoring-pn/rules',
};
@Injectable({
  providedIn: 'root'
})
export class MonitoringPnNotificationRulesService extends BaseService {

  constructor(private _http: HttpClient, router: Router, toastrService: ToastrService) {
    super(_http, router, toastrService);
  }

  getRulesList(model: MonitoringNotificationsRequestModel): Observable<OperationDataResult<MonitoringNotificationRulesListModel>> {
    return this.get(MonitoringPnNotificationRulesMethods.Base, model);
  }

  getRule(id: number): Observable<OperationDataResult<MonitoringNotificationRuleModel>> {
    return this.get(MonitoringPnNotificationRulesMethods.Base + '/' + id);
  }

  updateRule(model: MonitoringNotificationRuleModel): Observable<OperationResult> {
    return this.put(MonitoringPnNotificationRulesMethods.Base, model);
  }

  createRule(model: MonitoringNotificationRuleModel): Observable<OperationResult> {
    return this.post(MonitoringPnNotificationRulesMethods.Base, model);
  }

  deleteRule(id: number): Observable<OperationResult> {
    return this.delete(MonitoringPnNotificationRulesMethods.Base + '/' + id);
  }
}
