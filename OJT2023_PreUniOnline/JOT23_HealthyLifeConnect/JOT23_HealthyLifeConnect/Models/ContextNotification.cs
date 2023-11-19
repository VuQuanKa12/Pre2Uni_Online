namespace JOT23_Pre2UniOnline.Models
{
	public class ContextNotification
	{
		private List<Notification> _notifications;
		private ContextNotification instence;
		public ContextNotification Instance()
		{
			if (instence == null)
			{
				instence = new ContextNotification();
			}
			return instence;
		}
		public List<Notification> GetNotifications()
		{
			return _notifications;
		}

		public void addNontification(Notification notification)
		{
			_notifications.Add(notification);
		}
	}
}
