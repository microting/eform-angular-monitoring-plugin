import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import {
  NotificationRulesStore,
  NotificationRulesState,
} from './notification-rules-store';

@Injectable({ providedIn: 'root' })
export class NotificationRulesQuery extends Query<NotificationRulesState> {
  constructor(protected store: NotificationRulesStore) {
    super(store);
  }

  get pageSetting() {
    return this.getValue();
  }

  selectPageSize$ = this.select('pageSize');
  selectNameFilter$ = this.select('nameFilter');
  selectIsSortDsc$ = this.select('isSortDsc');
  selectSort$ = this.select('sort');
  selectOffset$ = this.select('offset');
}
