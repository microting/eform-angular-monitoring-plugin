import {Component, OnInit, ViewChild} from '@angular/core';
import {PageSettingsModel} from 'src/app/common/models/settings';

import {SharedPnService} from 'src/app/plugins/modules/shared/services';
import {MonitoringPnNotificationRulesService} from '../../../services';
import {
  NotificationRuleSimpleModel,
  NotificationRulesListModel,
  NotificationsRequestModel
} from '../../../models';
import {NotificationRulesDeleteComponent, NotificationRulesEditComponent} from '..';

@Component({
  selector: 'app-monitoring-pn-notification-rules-page',
  templateUrl: './notification-rules-page.component.html',
  styleUrls: ['./notification-rules-page.component.scss']
})
export class NotificationRulesPageComponent implements OnInit {
  @ViewChild('editRuleModal') editRuleModal: NotificationRulesEditComponent;
  @ViewChild('deleteRuleModal') deleteRuleModal: NotificationRulesDeleteComponent;
  localPageSettings: PageSettingsModel = new PageSettingsModel();
  rulesModel: NotificationRulesListModel = new NotificationRulesListModel();
  rulesRequestModel: NotificationsRequestModel = new NotificationsRequestModel();
  spinnerStatus = false;

  constructor(
    private sharedPnService: SharedPnService,
    private monitoringPnRulesService: MonitoringPnNotificationRulesService
  ) { }

  ngOnInit() {
    this.getLocalPageSettings();
  }

  getLocalPageSettings() {
    this.localPageSettings = this.sharedPnService
      .getLocalPageSettings('monitoringPnLocalSettings', 'NotificationRules')
      .settings;
    this.getRulesList();
  }

  updateLocalPageSettings() {
    this.sharedPnService.updateLocalPageSettings(
      'monitoringPnLocalSettings',
      this.localPageSettings,
      'NotificationRules'
    );
    this.getRulesList();
  }

  getRulesList() {
    this.spinnerStatus = true;
    this.rulesRequestModel.isSortDsc = this.localPageSettings.isSortDsc;
    this.rulesRequestModel.sort = this.localPageSettings.sort;
    this.rulesRequestModel.pageSize = this.localPageSettings.pageSize;

    this.monitoringPnRulesService.getRulesList(this.rulesRequestModel).subscribe((data) => {
      if (data && data.success) {
        this.rulesModel = data.model;
      }
      this.spinnerStatus = false;
    });
  }

  showEditRuleModal(id?: number) {
    this.editRuleModal.show(id);
  }

  showDeleteRuleModal(model: NotificationRuleSimpleModel) {
    this.deleteRuleModal.show(model);
  }

  sortTable(sort: string) {
    if (this.localPageSettings.sort === sort) {
      this.localPageSettings.isSortDsc = !this.localPageSettings.isSortDsc;
    } else {
      this.localPageSettings.isSortDsc = false;
      this.localPageSettings.sort = sort;
    }
    this.updateLocalPageSettings();
  }

  changePage(e: any) {
    if (e || e === 0) {
      this.rulesRequestModel.offset = e;
      if (e === 0) {
        this.rulesRequestModel.pageIndex = 0;
      } else {
        this.rulesRequestModel.pageIndex
          = Math.floor(e / this.rulesRequestModel.pageSize);
      }
      this.getRulesList();
    }
  }
}
