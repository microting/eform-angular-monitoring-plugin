import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {EFormService} from '../../../../../../common/services/eform';
import {SitesService} from '../../../../../../common/services/advanced';
import {AuthService} from 'src/app/common/services';
import {MonitoringNotificationRuleModel} from '../../../models';
import {MonitoringPnNotificationRulesService} from '../../../services';


@Component({
  selector: 'app-monitoring-pn-notification-rules-create',
  templateUrl: './notification-rules-create.component.html',
  styleUrls: ['./notification-rules-create.component.scss']
})
export class NotificationRulesCreateComponent implements OnInit {
  @ViewChild('frame') frame;
  @Output() ruleCreated: EventEmitter<void> = new EventEmitter<void>();
  spinnerStatus = false;
  newRuleModel: MonitoringNotificationRuleModel = new MonitoringNotificationRuleModel();
  typeahead = new EventEmitter<string>();

  get userClaims() {
    return this.authService.userClaims;
  }

  constructor(
    private trashInspectionPnRulesService: MonitoringPnNotificationRulesService,
    private sitesService: SitesService,
    private authService: AuthService,
    private eFormService: EFormService
  ) { }

  ngOnInit() {
  }

  createRule() {
    this.spinnerStatus = true;

    this.trashInspectionPnRulesService.createRule(this.newRuleModel).subscribe((data) => {
      if (data && data.success) {
        this.ruleCreated.emit();
        this.newRuleModel = new MonitoringNotificationRuleModel();
        this.frame.hide();
      } this.spinnerStatus = false;
    });
  }

  show() {
    this.frame.show();
  }
}
