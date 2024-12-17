declare
class Task
   attr id description dueDate priority status
   meth init(ID Desc DueDate Priority Status)
      id := ID
      description := Desc
      dueDate := DueDate
      priority := Priority
      status := Status
   end

   meth update(Status Priority)
      status := Status
      priority := Priority
   end

   meth display
      {Browse "Task id: "#@id}
   end

   meth getId(D)
      D = @id
   end

   meth getStatus(S)
      S = @status
   end

   meth getPriority(P)
   P = @priority
   end

   meth getDueDate(D)
      D = @dueDate
   end
end

%============================================
declare
Task1 Task2 List1 in

Task1 = {New Task init(1 "Completed first class"  date('Day':4 'Month':12 'Year':2024) 3 "Pending")}

Task2 = {New Task init(2 "Completed 2nd class" date('Day':5 'Month':12 'Year':2024) 1 "Pending")}
List1 = [Task1 Task2]

declare AppendTask AppendTask1 AppendTask2 in
AppendTask = {New Task init(3 "Complete assignment" date('Day':6 'Month':12 'Year':2024) 6 "Pending")}
AppendTask1 = {New Task init(4 "Complete assignment" date('Day':5 'Month':12 'Year':2024) 3 "Overdue")}
AppendTask2 = {New Task init(5 "Complete assignment" date('Day':8 'Month':12 'Year':2024) 1 "Completed")}


declare
fun {AppendElement L E}
   case L of nil then [E]
   [] H|T then H|{AppendElement T E}
   end
end

% Add the new task to the list
declare NewList in
NewList = {AppendElement List1 AppendTask}

% Add new tasks to the list
declare ExtendedList ExtendedList2 in
   ExtendedList = {AppendElement NewList AppendTask1}
   ExtendedList2 = {AppendElement ExtendedList AppendTask2}

%print list
declare
proc {PrintTaskList L}
   case L of nil then skip
   [] H|T then
      {H display}
      {PrintTaskList T}
   end
end



fun {DeleteById L IdToDelete}
   case L of nil then nil
   [] H|T then
      local TaskId in
         {H getId(TaskId)} 
         if TaskId == IdToDelete then
            {DeleteById T IdToDelete}  
         else
            H | {DeleteById T IdToDelete} 
         end
      end
   end
end



%{PrintTaskList NewList}
% {Browse 'List before deletion:'}
% {PrintTaskList NewList}

% {Browse 'List after deletion:'}
% {PrintTaskList {DeleteById NewList 2}}

%% ===========================

%% sorting algorithms 


fun {CompareByPriority Task1 Task2}
   local Priority1 Priority2 in
      {Task1 getPriority(Priority1)}
      {Task2 getPriority(Priority2)}
      Priority2 >= Priority1 
   end
end
declare PriorityComparison in
PriorityComparison = {CompareByPriority Task1 Task2}
% {Browse 'Task1 priority >= Task2 priority: ' # PriorityComparison}

declare
proc {Split L Half1 Half2}
   case L of nil then Half1 = nil Half2 = nil
   [] [X] then Half1 = [X] Half2 = nil
   [] X1|X2|T then
      local H1 H2 in
         {Split T H1 H2}
         Half1 = X1|H1
         Half2 = X2|H2
      end
   end
end
 
 %
% declare
% Half1 Half2 in
% {Split NewList Half1 Half2}

% {Browse 'Original List:'}
% {PrintTaskList NewList}

% {Browse 'First Half of the List:'}
% {PrintTaskList Half1}

% {Browse 'Second Half of the List:'}
% {PrintTaskList Half2}

declare
fun {Merge L1 L2}
   case L1 of 
      nil then L2
   [] H1|T1 then
      case L2 of 
         nil then L1
      [] H2|T2 then
         if {CompareByPriority H1 H2} then
            H1 | {Merge T1 L2}
         else
            H2 | {Merge L1 T2}
         end
      end
   end
end

declare
fun {MergeSort L}
   case L of 
      nil then nil
   [] [_] then L
   [] _ then
      local Half1 Half2 SortedHalf1 SortedHalf2 in
         {Split L Half1 Half2}
         SortedHalf1 = {MergeSort Half1}
         SortedHalf2 = {MergeSort Half2}
         {Merge SortedHalf1 SortedHalf2}
      end
   end
end
declare SortedList
SortedList = {MergeSort List1}
% {Browse 'Sorted List by Priority:'}
% {PrintTaskList SortedList}


%//////////////////////////////
% Filtering Algorithms


% Filter by Status
fun {FilterByStatus OldList Status}
   case OldList of nil then nil
   [] H|T then
      local TaskStatus in
         {H getStatus(TaskStatus)} 
         if TaskStatus == Status then
               H | {FilterByStatus T Status} 
         else
               {FilterByStatus T Status}    
         end
      end
   end
end

% Filter by Priority
fun {FilterByPriority OldList Priority}
   case OldList of nil then nil
   [] H|T then
      local TaskPriority in
         {H getPriority(TaskPriority)} 
         if TaskPriority == Priority then
               H | {FilterByPriority T Priority}  
         else
               {FilterByPriority T Priority}  
         end
      end
   end
end

% Filter by DueDate
fun {FilterByDueDate OldList DueDate}
   case OldList of nil then nil
   [] H|T then
      local TaskDueDate in
         {H getDueDate(TaskDueDate)} 
         if TaskDueDate == DueDate then
               H | {FilterByDueDate T DueDate}  
         else
               {FilterByDueDate T DueDate}  
         end
      end
   end
end 

% Filtering and Printing
declare FilteredByStatusList FilteredByPriorityList FilteredByDueDateList in
FilteredByStatusList = {FilterByStatus ExtendedList2 "Completed"}
FilteredByPriorityList = {FilterByPriority ExtendedList2 1}
FilteredByDueDateList = {FilterByDueDate ExtendedList2 [4 12 2024]}

% {Browse "Filtering by Status 'Completed': "}
% {PrintTaskList FilteredByStatusList}

% {Browse "Filtering by Priority 1 : "}
% {PrintTaskList FilteredByPriorityList}

% {Browse "Filtering by Due Date '2024-12-04': "}
% {PrintTaskList FilteredByDueDateList}


%/////////////////////////////////
% Notifications: 
% 1. Highlight tasks nearing deadlines.
% Highlight tasks nearing their deadlines (tasks where due date matches current date)

