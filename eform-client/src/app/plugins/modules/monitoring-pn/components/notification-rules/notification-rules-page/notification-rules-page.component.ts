import { Component, OnInit, ViewChild } from '@angular/core';
import {
  NotificationRuleSimpleModel,
  NotificationRulesListModel,
} from '../../../models';
import {
  NotificationRulesDeleteComponent,
  NotificationRulesEditComponent,
} from '..';
import { MonitoringPnClaims } from '../../../const/monitoring-pn-claims.const';
import { DeviceUserService } from 'src/app/common/services';
import { SiteDto, TableHeaderElementModel } from 'src/app/common/models';
import { NotificationRulesStateService } from '../store/notification-rules-state-service';
import { AuthStateService } from 'src/app/common/store';

@Component({
  selector: 'app-monitoring-pn-notification-rules-page',
  templateUrl: './notification-rules-page.component.html',
  styleUrls: ['./notification-rules-page.component.scss'],
})
export class NotificationRulesPageComponent implements OnInit {
  @ViewChild('editRuleModal', { static: false })
  editRuleModal: NotificationRulesEditComponent;
  @ViewChild('deleteRuleModal', { static: false })
  deleteRuleModal: NotificationRulesDeleteComponent;
  rulesModel: NotificationRulesListModel = new NotificationRulesListModel();
  sitesDto: SiteDto[];

  tableHeaders: TableHeaderElementModel[] = [
    { name: 'Id', elementId: 'idTableHeader', sortable: true },
    {
      name: 'eFormName',
      elementId: 'eFormNameTableHeader',
      sortable: true,
      visibleName: 'eForm Name',
    },
    { name: 'Trigger', elementId: 'triggerTableHeader', sortable: true },
    { name: 'Event', elementId: 'eventTableHeader', sortable: true },
    this.authStateService.checkClaim(
      this.monitoringPnClaims.updateNotificationRules
    ) ||
    this.authStateService.checkClaim(
      this.monitoringPnClaims.deleteNotificationRules
    )
      ? { name: 'Actions', elementId: '', sortable: false }
      : null,
  ];

  constructor(
    private deviceUsersService: DeviceUserService,
    public notificationRulesStateService: NotificationRulesStateService,
    public authStateService: AuthStateService
  ) {}

  get monitoringPnClaims() {
    return MonitoringPnClaims;
  }

  ngOnInit() {
    this.getRulesList();
    this.loadAllSimpleSites();
  }

  getRulesList() {
    this.notificationRulesStateService.getRulesList().subscribe((data) => {
      if (data && data.success) {
        this.rulesModel = data.model;
      }
    });
  }

  // showEditRuleModal(id?: number) {
  //   this.editRuleModal.show(id);
  // }

  showDeleteRuleModal(model: NotificationRuleSimpleModel) {
    this.deleteRuleModal.show(model);
  }

  sortTable(sort: string) {
    this.notificationRulesStateService.onSortTable(sort);
    this.getRulesList();
  }

  changePage(offset: number) {
    this.notificationRulesStateService.changePage(offset);
    this.getRulesList();
  }

  loadAllSimpleSites() {
    this.deviceUsersService.getAllDeviceUsers().subscribe((operation) => {
      if (operation && operation.success) {
        this.sitesDto = operation.model.map((i) => {
          i.fullName = i.siteName;
          return i;
        });

        // this.sitesDto = operation.model.map(x => ({...x, fullName: `${x.firstName} ${x.lastName}`}));
      }
    });
  }

  onPageSizeChanged(pageSize: number) {
    this.notificationRulesStateService.updatePageSize(pageSize);
    this.getRulesList();
  }

  ruleDeleted() {
    this.notificationRulesStateService.onDelete();
    this.getRulesList();
  }
}
