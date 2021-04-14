import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';

export interface NotificationRulesState {
  pageSize: number;
  sort: string;
  isSortDsc: boolean;
  nameFilter: string;
  offset: number;
}

export function createInitialState(): NotificationRulesState {
  return {
    pageSize: 10,
    sort: 'Id',
    isSortDsc: false,
    nameFilter: '',
    offset: 0,
  };
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'notificationRules' })
export class NotificationRulesStore extends Store<NotificationRulesState> {
  constructor() {
    super(createInitialState());
  }
}
