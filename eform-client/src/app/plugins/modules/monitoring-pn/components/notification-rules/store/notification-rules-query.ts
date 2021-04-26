import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import {
  NotificationRulesState,
  NotificationRulesStore,
} from './notification-rules-store';

@Injectable({ providedIn: 'root' })
export class NotificationRulesQuery extends Query<NotificationRulesState> {
  constructor(protected store: NotificationRulesStore) {
    super(store);
  }

  get pageSetting() {
    return this.getValue();
  }

  selectNameFilter$ = this.select((state) => state.pagination.nameFilter);
  selectPageSize$ = this.select((state) => state.pagination.pageSize);
  selectIsSortDsc$ = this.select((state) => state.pagination.isSortDsc);
  selectSort$ = this.select((state) => state.pagination.sort);
  selectOffset$ = this.select((state) => state.pagination.offset);
}
