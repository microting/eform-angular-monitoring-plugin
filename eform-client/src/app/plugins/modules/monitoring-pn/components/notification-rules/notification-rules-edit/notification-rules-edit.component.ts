import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {EFormService} from '../../../../../../common/services/eform';
import {MonitoringPnNotificationRulesService} from '../../../services';
import {MonitoringNotificationRuleModel} from '../../../models';

@Component({
  selector: 'app-monitoring-pn-notification-rules-edit',
  templateUrl: './notification-rules-edit.component.html',
  styleUrls: ['./notification-rules-edit.component.scss']
})
export class NotificationRulesEditComponent implements OnInit {
  @ViewChild('frame') frame;
  @Output() ruleSaved: EventEmitter<void> = new EventEmitter<void>();
  spinnerStatus = false;
  ruleModel: MonitoringNotificationRuleModel = new MonitoringNotificationRuleModel();
  typeahead = new EventEmitter<string>();

  constructor(
    private monitoringRulesService: MonitoringPnNotificationRulesService,
    private eFormService: EFormService
  ) { }

  ngOnInit() {
  }

  show(id?: number) {
    if (id) {
      this.getSelectedRule(id);
    } else {
      this.ruleModel = {
        id: null,
        attachReport: false,
        data: {},
        dataItemId: null,
        recipients: [],
        ruleType: null,
        subject: '',
        templateId: null,
        text: ''
      };
    }
    this.frame.show();
  }

  getSelectedRule(id: number) {
    this.spinnerStatus = true;
    this.monitoringRulesService.getRule(id).subscribe((data) => {
      if (data && data.success) {
        this.ruleModel = data.model;

      } this.spinnerStatus = false;
    });
  }

  saveRule() {
    this.spinnerStatus = true;

    if (this.ruleModel.id) {
      this.monitoringRulesService.updateRule(this.ruleModel).subscribe((data) => {
        if (data && data.success) {
          this.ruleSaved.emit();
          this.ruleModel = new MonitoringNotificationRuleModel();
          this.frame.hide();
        }
        this.spinnerStatus = false;
      });
    } else {
      this.monitoringRulesService.createRule(this.ruleModel).subscribe((data) => {
        if (data && data.success) {
          this.ruleSaved.emit();
          this.ruleModel = new MonitoringNotificationRuleModel();
          this.frame.hide();
        }
        this.spinnerStatus = false;
      });
    }
  }
}
