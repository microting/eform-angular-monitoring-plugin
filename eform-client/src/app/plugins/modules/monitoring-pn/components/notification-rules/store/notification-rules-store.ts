import { Injectable } from '@angular/core';
import { persistState, Store, StoreConfig } from '@datorama/akita';
import { CommonPaginationState } from 'src/app/common/models/common-pagination-state';

export interface NotificationRulesState {
  pagination: CommonPaginationState;
}

export function createInitialState(): NotificationRulesState {
  return <NotificationRulesState>{
    pagination: {
      pageSize: 10,
      sort: 'Id',
      isSortDsc: false,
      nameFilter: '',
      offset: 0,
    },
  };
}

const notificationRulesPersistStorage = persistState({
  include: ['monitoringPnNotificationRules'],
  key: 'pluginsStore',
});

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'monitoringPnNotificationRules', resettable: true })
export class NotificationRulesStore extends Store<NotificationRulesState> {
  constructor() {
    super(createInitialState());
  }
}

export const notificationRulesPersistProvider = {
  provide: 'persistStorage',
  useValue: notificationRulesPersistStorage,
  multi: true,
};
