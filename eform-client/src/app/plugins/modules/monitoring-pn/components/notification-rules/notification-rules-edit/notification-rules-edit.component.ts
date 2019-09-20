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
  @Output() ruleUpdated: EventEmitter<void> = new EventEmitter<void>();
  spinnerStatus = false;
  selectedRuleModel: MonitoringNotificationRuleModel = new MonitoringNotificationRuleModel();
  typeahead = new EventEmitter<string>();

  constructor(
    private monitoringRulesService: MonitoringPnNotificationRulesService,
    private eFormService: EFormService
  ) { }

  ngOnInit() {
  }

  show(ruleModel: MonitoringNotificationRuleModel) {
    this.getSelectedRule(ruleModel.id);
    this.frame.show();
  }

  getSelectedRule(id: number) {
    this.spinnerStatus = true;
    this.monitoringRulesService.getRule(id).subscribe((data) => {
      if (data && data.success) {
        this.selectedRuleModel = data.model;

      } this.spinnerStatus = false;
    });
  }

  updateRule() {
    this.spinnerStatus = true;
    const model = this.selectedRuleModel;

    this.monitoringRulesService.updateRule(model).subscribe((data) => {
      if (data && data.success) {
        this.ruleUpdated.emit();
        this.selectedRuleModel = new MonitoringNotificationRuleModel();
        this.frame.hide();
      } this.spinnerStatus = false;
    });
  }
}
