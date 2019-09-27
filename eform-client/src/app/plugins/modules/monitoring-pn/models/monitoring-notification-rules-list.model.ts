export class MonitoringNotificationRulesListModel {
  total: number;
  rules: MonitoringNotificationRuleSimpleModel[] = [];
}

export class MonitoringNotificationRuleSimpleModel {
  id: number;
  trigger: string;
  event: string;
}
