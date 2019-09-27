import {MonitoringRuleType} from '../enums/monitoring-rule-type.enum';
import {MonitoringRecipientModel} from './monitoring-recipient.model';

export class MonitoringNotificationRuleModel {
  id: number;
  templateId: number;
  subject: string;
  text: string;
  attachReport: boolean;
  ruleType: MonitoringRuleType;
  dataItemId: number;
  data: object;
  recipients: MonitoringRecipientModel[];
}
