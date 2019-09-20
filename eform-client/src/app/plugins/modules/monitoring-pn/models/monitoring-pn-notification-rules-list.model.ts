export class MonitoringPnNotificationRulesListModel {
  total: number;
  rules: MonitoringPnNotificationRuleSimpleModel[] = [];
}

export class MonitoringPnNotificationRuleSimpleModel {
  id: number;
  trigger: string;
  event: string;
}
