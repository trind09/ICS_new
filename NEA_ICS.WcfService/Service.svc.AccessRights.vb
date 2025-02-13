﻿Imports NEA_ICS.Business
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Class Service

#Region " Access Rights "

    ''' <summary>
    ''' Function - GetStoreAccess;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStoreAccess(ByVal roleDetails As RoleDetails) As List(Of StoreDetails) _
                                   Implements IService.GetStoreAccess

        Dim StoreAccessList As New List(Of StoreDetails)

        Try

            Dim StoreAccessRetrieved As New DataSet
            StoreAccessList.Clear()

            StoreAccessRetrieved = AccessRightsBL.GetStoreAccess(roleDetails.UserID)

            If StoreAccessRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In StoreAccessRetrieved.Tables(0).Rows
                    row = FillRowWithNull(row, StoreAccessRetrieved.Tables(0).Columns)

                    Dim StoreDetailsItem As New StoreDetails

                    StoreDetailsItem.StoreId = row("UserRoleStoreID")
                    StoreDetailsItem.StoreName = row("StoreName")

                    StoreAccessList.Add(StoreDetailsItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return StoreAccessList

    End Function

    ''' <summary>
    ''' Function - GetUserProfile;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserProfile(ByVal roleDetails As RoleDetails) As List(Of RoleDetails) _
                                     Implements IService.GetUserProfile

        Dim UserProfileList As New List(Of RoleDetails)

        Try

            Dim UserRoleRetrieved As New DataSet
            UserProfileList.Clear()

            UserRoleRetrieved = AccessRightsBL.GetUserProfile(roleDetails.StoreID, roleDetails.UserID)

            If UserRoleRetrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In UserRoleRetrieved.Tables(0).Rows

                    Dim UserProfileItem As New RoleDetails

                    UserProfileItem.UserID = row("VUserProfileUserID")
                    UserProfileItem.Name = row("VUserProfileName")
                    UserProfileItem.Designation = row("VUserProfileDesignation")
                    UserProfileItem.UserRole = row("RoleType")
                    UserProfileItem.LastLogin = row("LastLoginDte")
                    UserProfileItem.LastLogout = IIf(IsDBNull(row("LastLogoutDte")), DateTime.MinValue, row("LastLogoutDte"))

                    UserProfileList.Add(UserProfileItem)

                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserProfileList

    End Function

    ''' <summary>
    ''' Function - GetUserStoreCodes;
    ''' 20 Sep 09 - Christina;
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserStoreCodes(ByVal userId As String) As String _
                                     Implements IService.GetUserStoreCodes

        Dim storeCodes As String = ""
        Try
            storeCodes = AccessRightsBL.GetUserStoreCodes(userId)
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return storeCodes
    End Function

    ''' <summary>
    ''' Function - CheckConcurrentLogin;
    ''' 25 Aug 11 - Christina;
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="checkIfLogout"></param>
    ''' <param name="sessionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserLogins(ByVal userID As String, ByVal sessionId As String, ByVal checkIfLogout As Boolean) As Integer _
                                     Implements IService.GetUserLogins

        Dim NumLogins As Integer
        Try

            NumLogins = AccessRightsBL.GetUserLogins(userID, sessionId, checkIfLogout)

            'If checkIfLogout Then ' check if user is kicked-out
            '    If NumLogins = 0 Then ' if user session is not found

            '    End If
            'Else
            '    If NumLogins = 0 Then ' if user session is not found

            '    End If
            'End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return NumLogins
    End Function

    ' ''' <summary>
    ' ''' Function - AddUserAudit;
    ' ''' 02 Sept 13 - Christina;
    ' ''' </summary>
    ' ''' <param name="roleDetails"></param>
    ' ''' <param name="sessionID"></param>
    ' ''' <param name="isNonIcsUser"></param>
    ' ''' <param name="isInactiveUser"></param>
    ' ''' <param name="isUnsuccessfulLogin"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    Public Function AddUserAudit(ByVal storeId As String, ByVal userId As String _
                                    , ByVal userIP As String, ByVal sessionID As String _
                                    , Optional ByVal isNonIcsUser As Boolean = False _
                                    , Optional ByVal isInactiveUser As Boolean = False _
                                    , Optional ByVal isUnsuccessfulLogin As Boolean = False) As String _
                                 Implements IService.AddUserAudit

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = AccessRightsBL.AddUserAudit(storeId, userId _
                                                        , userIP, sessionID _
                                                        , isNonIcsUser, isInactiveUser, isUnsuccessfulLogin)

        Catch ex As Exception

            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Function - UpdateUserAudit;
    ''' 16 Feb 09 - Jianfa;
    ''' </summary>
    ''' <param name="roleDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateUserAudit(ByVal roleDetails As RoleDetails, ByVal sessionId As String) As String Implements IService.UpdateUserAudit

        Dim errorMessage As String = String.Empty

        Try

            errorMessage = AccessRightsBL.UpdateUserAudit(roleDetails.StoreID, roleDetails.UserID, sessionId)

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage

    End Function

    ''' <summary>
    ''' Manage Financial Closing, update past finanical cutoff date record details;
    ''' 28Feb09 - KG;
    ''' </summary>
    ''' <param name="storeID"></param>
    ''' <param name="loginUser"></param>
    ''' <remarks>
    ''' CHANGE LOG:
    ''' ddMMMyy  AuthorName  RefID  Description;
    ''' </remarks>
    Public Sub ManageFinancialClosing(ByVal storeID As String, ByVal loginUser As String) Implements IService.ManageFinancialClosing
        Try
            AccessRightsBL.ManageFinancialClosing(storeID _
                                                  , loginUser _
                                                  )

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Check if such User ID exist
    ''' Christina - 02 Sep 2013
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckUserIdExist(ByVal userID As String) As Boolean _
                        Implements IService.CheckUserIdExist
        Dim userExist As Boolean = False
        Try
            userExist = AccessRightsBL.CheckUserIdExist(userID)
            Return userExist
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return userExist
    End Function

    ' ''' <summary>
    ' ''' Manage Financial Closing, update past finanical cutoff date record details;
    ' ''' 28Feb09 - KG;
    ' ''' </summary>
    ' ''' <param name="storeID"></param>
    ' ''' <param name="userId"></param>
    ' ''' <remarks>
    ' ''' CHANGE LOG:
    ' ''' ddMMMyy  AuthorName  RefID  Description;
    ' ''' </remarks>
    Public Function ManageInactiveUser(ByVal storeID As String) As String _
                                                        Implements IService.ManageInactiveUser
        Dim errorMessage As String = String.Empty
        Try
            errorMessage = AccessRightsBL.ManageInactiveUser(storeID)

            'Dim lastLoginDate As DateTime
            'lastLoginDate = AccessRightsBL.GetLastLoginDate(storeID, userId)

            'Dim dayDifference As Integer
            'dayDifference = DateDiff(DateInterval.Day, lastLoginDate, Today)

            'If dayDifference > 90 Then ' if user last login date (regardless of role) > 90 days then deactivate user

            '    'Dim changeStatusReason As String = "User is Inactive for more than 90 days"
            '    errorMessage = AccessRightsBL.ManageInactiveUser(storeID)

            'End If
        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return errorMessage
    End Function

    ''' <summary>
    ''' Check User Roles status - inactive or deleted
    ''' Christina - 02 Sep 2013
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserRoleStatus(ByVal storeID As String, ByVal userID As String) As List(Of RoleDetails) _
                        Implements IService.GetUserRoleStatus

        Dim Retrieved As New DataSet
        Dim UserRoleStatusList As New List(Of RoleDetails)
        Dim RoleDetails As RoleDetails

        UserRoleStatusList.Clear()

        Retrieved = AccessRightsBL.GetUserRoleStatus(storeID, userID)

        Try
            If Retrieved.Tables(0).Rows.Count > 0 Then

                RoleDetails = New RoleDetails
                Dim RetrievedView As New DataView(Retrieved.Tables(0))
                Dim userStatus As String
                Dim isDeleted As Boolean
                'RetrievedView.RowFilter = "UserRoleStatus = 'O'"

                Dim foundRows() As DataRow
                foundRows = Retrieved.Tables(0).Select("UserRoleStatus = 'O'")
                If foundRows.Count > 0 Then ' if one of user's roles is active, dont tag user as inactive ics user
                    userStatus = "Active"
                Else 'if all user's roles is inactive, tag user as inactive ics user
                    userStatus = "Inactive"
                End If

                foundRows = Retrieved.Tables(0).Select("IsUserDeleted = 1")
                If foundRows.Count > 0 Then ' once user is deleted, all his roles is updated to deleted
                    isDeleted = True
                Else
                    isDeleted = False
                End If

                'RetrievedView.Sort = "UserRoleStatus"
                'RetrievedView.Find("O")
                'If RetrievedView.Find("O") > 0 Then ' if one of user's roles is active, dont tag user as inactive ics user
                '    userStatus = "Active"
                'Else 'if all user's roles is inactive, tag user as inactive ics user
                '    userStatus = "Inactive"
                'End If

                'RetrievedView.Sort = "IsUserDeleted"
                'If RetrievedView.Find(True) > 0 Then ' once user is deleted, all his roles is updated to deleted
                '    isDeleted = True
                'Else
                '    isDeleted = False
                'End If

                For Each viewRow As DataRowView In RetrievedView
                    viewRow = FillViewRowWithNull(viewRow, RetrievedView.Table.Columns)
                    RoleDetails.StoreID = viewRow("UserRoleStoreID")
                    RoleDetails.UserID = viewRow("UserRoleUserID")
                    RoleDetails.UserRole = viewRow("UserRoleCode")
                    RoleDetails.UserRole = viewRow("UserRoleDescription")
                    RoleDetails.Status = userStatus
                    RoleDetails.IsUserDeleted = isDeleted
                    RoleDetails.ChangeStatusReason = viewRow("ChangeStatusReason")
                    UserRoleStatusList.Add(RoleDetails)
                Next
            End If

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then
                Throw
            End If
        End Try

        Return UserRoleStatusList
    End Function
#End Region
#Region " User Activities "
    Public Function GetUserActivityList(ByVal storeID As String _
                          , ByVal userId As String _
                          , ByVal fromDte As Date _
                          , ByVal toDte As Date _
                          , ByVal byTimeStamp As Boolean _
                          , ByVal filterBy As String _
                          , ByVal sortBy As String _
                          ) As List(Of RoleDetails) Implements IService.GetUserActivityList
        Try
            Dim List As New List(Of RoleDetails)
            Dim RoleDetails As RoleDetails
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = AccessRightsBL.GetUserActivityList(storeID _
                                                    , userId _
                                                    , fromDte _
                                                    , toDte _
                                                    , byTimeStamp _
                                                    )

            If sortBy = String.Empty Then
                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In Retrieved.Tables(0).Rows
                        row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                        RoleDetails = New RoleDetails

                        RoleDetails.StoreID = row("AUserStoreID")
                        RoleDetails.UserID = row("AUserID")
                        RoleDetails.LastLogin = row("AUserLoginDte")
                        RoleDetails.UserIP = row("AUserIP")
                        RoleDetails.Name = row("VUserProfileName")

                        List.Add(RoleDetails)
                    Next
                End If
            Else
                Dim RetrievedView As New DataView(Retrieved.Tables(0))
                RetrievedView.Sort = sortBy

                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In RetrievedView
                        viewRow = FillViewRowWithNull(viewRow, RetrievedView.Table.Columns)
                        RoleDetails = New RoleDetails

                        RoleDetails.StoreID = viewRow("AUserStoreID")
                        RoleDetails.UserID = viewRow("AUserID")
                        RoleDetails.LastLogin = viewRow("AUserLoginDte")
                        RoleDetails.UserIP = viewRow("AUserIP")
                        RoleDetails.Name = viewRow("VUserProfileName")

                        List.Add(RoleDetails)
                    Next
                End If
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region

#Region " User Unsuccessful Logins "
    Public Function GetUserUnsuccessfulLoginList(ByVal storeID As String _
                          , ByVal userId As String _
                          , ByVal fromDte As Date _
                          , ByVal toDte As Date _
                          , ByVal sortBy As String _
                          ) As List(Of RoleDetails) Implements IService.GetUserUnsuccessfulLoginList
        Try
            Dim List As New List(Of RoleDetails)
            Dim RoleDetails As RoleDetails
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = AccessRightsBL.GetUserUnsuccessfulLoginList(storeID _
                                                    , userId _
                                                    , fromDte _
                                                    , toDte _
                                                    )

            If sortBy = String.Empty Then
                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In Retrieved.Tables(0).Rows
                        row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                        RoleDetails = New RoleDetails

                        'Dim ds As New DataSet
                        'Dim roles As String = ""
                        'If row("AUserStoreID") Is String.Empty Then
                        '    ds = AccessRightsBL.GetStoreAccess(row("AUserID"))
                        '    If ds.Tables(0).Rows.Count > 0 Then
                        '        For Each roleRow As DataRow In ds.Tables(0).Rows
                        '            roles = roleRow("UserRoleStoreID") & ","
                        '        Next
                        '    End If
                        '    RoleDetails.UserRole = roles
                        'Else
                        '    RoleDetails.UserRole = row("AUserStoreID")
                        'End If

                        RoleDetails.UserRole = row("AUserStoreID")
                        RoleDetails.StoreID = row("AUserStoreID")
                        RoleDetails.UserID = row("AUserID")
                        RoleDetails.LastLogin = row("AUserLoginDte")
                        RoleDetails.UserIP = row("AUserIP")
                        RoleDetails.Name = row("VUserProfileName")

                        List.Add(RoleDetails)
                    Next
                End If
            Else
                Dim RetrievedView As New DataView(Retrieved.Tables(0))
                RetrievedView.Sort = sortBy

                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In RetrievedView
                        viewRow = FillViewRowWithNull(viewRow, RetrievedView.Table.Columns)
                        RoleDetails = New RoleDetails

                        RoleDetails.StoreID = viewRow("AUserStoreID")
                        RoleDetails.UserID = viewRow("AUserID")
                        RoleDetails.LastLogin = viewRow("AUserLoginDte")
                        RoleDetails.UserIP = viewRow("AUserIP")
                        RoleDetails.Name = viewRow("VUserProfileName")

                        List.Add(RoleDetails)
                    Next
                End If
            End If

            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region

#Region " New user accounts created "
    Public Function GetNewUserAccountList(ByVal storeID As String _
                          , ByVal fromDte As Date _
                          , ByVal toDte As Date _
                          , ByVal sortBy As String _
                          ) As List(Of RoleDetails) Implements IService.GetNewUserAccountList
        Try
            Dim List As New List(Of RoleDetails)
            Dim RoleDetails As RoleDetails
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = AccessRightsBL.GetNewUserAccountList(storeID _
                                                    , fromDte _
                                                    , toDte _
                                                    )

            If sortBy = String.Empty Then
                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In Retrieved.Tables(0).Rows
                        row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                        RoleDetails = New RoleDetails

                        RoleDetails.StoreID = row("UserRoleStoreID")
                        RoleDetails.UserID = row("VUserProfileUserID")
                        RoleDetails.CreatedDate = row("UserRoleCreateDte")
                        RoleDetails.CreatedBy = row("UserRoleCreateUserID")
                        RoleDetails.Name = row("VUserProfileName")
                        RoleDetails.UserStatus = row("UserRoleStatus")

                        List.Add(RoleDetails)
                    Next
                End If
            Else
                Dim RetrievedView As New DataView(Retrieved.Tables(0))
                RetrievedView.Sort = sortBy

                If Retrieved.Tables(0).Rows.Count > 0 Then
                    For Each viewRow As DataRowView In RetrievedView
                        viewRow = FillViewRowWithNull(viewRow, RetrievedView.Table.Columns)
                        RoleDetails = New RoleDetails

                        RoleDetails.StoreID = viewRow("UserRoleStoreID")
                        RoleDetails.UserID = viewRow("VUserProfileUserID")
                        RoleDetails.CreatedDate = viewRow("UserRoleCreateDte")
                        RoleDetails.CreatedBy = viewRow("UserRoleCreateUserID")
                        RoleDetails.Name = viewRow("VUserProfileName")

                        List.Add(RoleDetails)
                    Next
                End If
            End If

            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region

#Region " Non-ics unsuccessful logins"
    Public Function GetNonIcsUserUnsuccessfulLogin(ByVal storeID As String _
                          , ByVal fromDte As Date _
                          , ByVal toDte As Date _
                          ) As List(Of RoleDetails) Implements IService.GetNonIcsUserUnsuccessfulLogin
        Try
            Dim List As New List(Of RoleDetails)
            Dim RoleDetails As RoleDetails
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = AccessRightsBL.GetNonIcsUserUnsuccessfulLogin(storeID _
                                                    , fromDte _
                                                    , toDte _
                                                    )

            If Retrieved.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    RoleDetails = New RoleDetails

                    RoleDetails.UserID = row("AUserID")
                    RoleDetails.LastLogin = row("AUserLoginDte")
                    RoleDetails.UserIP = IIf(IsDBNull(row("AUserIP")), "", row("AUserIP"))
                    RoleDetails.ChangeStatusReason = IIf(IsDBNull(row("ChangeStatusReason")), "", row("ChangeStatusReason"))
                    List.Add(RoleDetails)
                Next
            End If

            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region

#Region " inactive user accounts "
    Public Function GetInactiveUsers(ByVal storeID As String, ByVal dateFrom As DateTime, ByVal dateTo As DateTime _
                          ) As List(Of RoleDetails) Implements IService.GetInactiveUsers
        Try
            Dim List As New List(Of RoleDetails)
            Dim RoleDetails As RoleDetails
            Dim Retrieved As New DataSet

            List.Clear()

            Retrieved = AccessRightsBL.GetInactiveUsers(storeID, dateFrom, dateTo)

            If Retrieved.Tables(0).Rows.Count > 0 Then
                Dim lastLoginDate As DateTime

                For Each row As DataRow In Retrieved.Tables(0).Rows
                    row = FillRowWithNull(row, Retrieved.Tables(0).Columns)
                    RoleDetails = New RoleDetails

                    RoleDetails.StoreID = row("UserRoleStoreID")
                    RoleDetails.UserID = row("VUserProfileUserID")
                    RoleDetails.UserRole = row("UserRoleCode")
                    RoleDetails.Status = row("UserRoleStatus")
                    RoleDetails.ChangeStatusReason = row("ChangeStatusReason")
                    lastLoginDate = AccessRightsBL.GetLastLoginDate(storeID, row("VUserProfileUserID"))
                    RoleDetails.LastLogin = lastLoginDate
                    'RoleDetails.CreatedBy = row("UserRoleCreateUserID")
                    RoleDetails.Name = row("VUserProfileName")
                    RoleDetails.UpdatedDate = row("UserRoleUpdateDte")


                    List.Add(RoleDetails)
                Next
            End If
            Return List

        Catch ex As Exception
            Dim rethrow As Boolean = ExceptionPolicy.HandleException(ex, "Service Policy")
            If (rethrow) Then Throw

            Return Nothing
        End Try
    End Function
#End Region


End Class
